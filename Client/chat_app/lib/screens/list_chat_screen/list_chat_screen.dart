import 'package:chat_app/models/message.dart' as mes;
import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/providers/message_provider.dart';
import 'package:chat_app/screens/list_chat_screen/list_chat_body.dart';
import 'package:chat_app/widgets/search/search_appbar_delegate.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:signalr_core/signalr_core.dart';

class ListChatScreen extends StatefulWidget {
  static const routeName = "/chat-list";
  const ListChatScreen({Key? key}) : super(key: key);

  @override
  _ListChatScreenState createState() => _ListChatScreenState();
}

class _ListChatScreenState extends State<ListChatScreen> {
  final connection = HubConnectionBuilder()
      .withUrl(
          'http://10.0.3.2:9999/hubChat',
          HttpConnectionOptions(
              logging: (level, message) => print(message),
              accessTokenFactory: () async {
                SharedPreferences sharedPred =
                    await SharedPreferences.getInstance();

                return sharedPred.getString("access_token");
              },
              withCredentials: false))
      .withAutomaticReconnect()
      .build();

  var init = false;
  late SearchAppBarDelegate _searchDelegate;
  @override
  void initState() {
    super.initState();
    print('intit');
    connectSignalR();
    _searchDelegate = SearchAppBarDelegate();
  }

  void connectSignalR() async {
    SharedPreferences sharePrf = await SharedPreferences.getInstance();
    final uid = sharePrf.getString('uid');

    if (connection.state != HubConnectionState.connected) {
      await connection.start();

      connection.on("NhanMessage", (newMessage) {
        print('nhan tin nhan moi $newMessage');

        final messageObj = mes.Message(
            messageId: newMessage![0]['id'],
            messageGroupChatId: newMessage[0]['groupId'],
            messageUserId: newMessage[0]['userId'],
            messengerUserName: newMessage[0]['userName'],
            messengerUserAvatar: newMessage[0]['userAvatar'],
            messageContent: newMessage[0]['message'],
            messageCreatedAt: DateTime.parse(newMessage[0]['timeSend']),
            isSender: newMessage[0]['userId'] == uid);

        Provider.of<MessageProvider>(context, listen: false)
            .newMessage(messageObj);

        Provider.of<GroupChat>(context, listen: false).newMessage(messageObj);
      });

      connection.on("NewGroupChat", (newGroupChat) {
        print('co group chat moi $newGroupChat');

        final groupChatObj = GroupChatModel(
            groupChatId: newGroupChat![0]['groupChatId'],
            groupChatName: newGroupChat[0]['groupChatName'],
            groupChatUpdatedAt:
                DateTime.parse(newGroupChat[0]['groupChatUpdatedAt']),
            groupAvatar: newGroupChat[0]['groupAvatar'],
            lastestMes: newGroupChat[0]['lastestMes']);

        Provider.of<GroupChat>(context, listen: false)
            .newGroupChat(groupChatObj);
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: buildAppBar(),
      body: ListChatBody(),
    );
  }

  AppBar buildAppBar() {
    return AppBar(
      title: Text("Chats"),
      automaticallyImplyLeading: false,
      actions: [
        IconButton(
            onPressed: () async {
              SharedPreferences sharePrf =
                  await SharedPreferences.getInstance();
              connection.stop();

              sharePrf.remove('uid');
              sharePrf.remove("access_token");

              Navigator.of(context).pushReplacementNamed('/');
            },
            icon: Icon(Icons.logout)),
        IconButton(
          onPressed: () {
            _showSearchPage(context, _searchDelegate);
          },
          icon: Icon(Icons.search),
        )
      ],
    );
  }

  void _showSearchPage(
      BuildContext context, SearchAppBarDelegate searchDelegate) {
    showSearch<String>(
      context: context,
      delegate: searchDelegate,
    );
  }
}
