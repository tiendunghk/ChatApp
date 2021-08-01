import 'dart:convert';

import 'package:chat_app/models/api_response.dart';
import 'package:chat_app/models/pagination_wrap.dart';
import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/providers/message_provider.dart';
import 'package:chat_app/screens/messages_screen/message.dart/message.dart';
import 'package:http/http.dart' as http;
import 'package:chat_app/screens/list_chat_screen/list_chat_body.dart';
import 'package:chat_app/signalr/chatHub.dart';
import 'package:chat_app/widgets/search/search_appbar_delegate.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:chat_app/models/message.dart' as mes;
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
    _searchDelegate = SearchAppBarDelegate();
  }

  @override
  void didChangeDependencies() {
    if (!init) {
      connectSignalR();
      init = true;
    }
    super.didChangeDependencies();
  }

  void connectSignalR() async {
    //ChatHub().connect();

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

        print('zzzzzzzzzzzzzzzzzzzzzzz: ${messageObj.messageCreatedAt}');

        Provider.of<MessageProvider>(context, listen: false)
            .newMessage(messageObj);

        Provider.of<GroupChat>(context, listen: false)
            .newMessage(messageObj);
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

  void _testPaging() async {
    SharedPreferences sharePrf = await SharedPreferences.getInstance();
    final token = sharePrf.getString('access_token');

    http.Response response = await http.get(
      Uri.parse(
          'http://10.0.3.2:9999/api/message?groupId=5a15e3e0-8b03-49e6-8b60-334cf32cd8dd&userId=acc1'),
      headers: {'Authorization': 'Bearer $token'},
    );

    if (response.statusCode == 200) {
      var jsonResponse = ApiResponse.fromJson(jsonDecode(response.body));

      var pagingWrapItems = PaginationWrap.fromJson(jsonResponse.data).items;

      var messages = List<mes.Message>.from(
          pagingWrapItems.map((model) => mes.Message.fromJson(model)));
      print('chieu dai la : ${messages.length}');
    }
  }

  AppBar buildAppBar() {
    return AppBar(
      title: Text("Chats"),
      automaticallyImplyLeading: false,
      actions: [
        IconButton(
            onPressed: () {
              _testPaging();
            },
            icon: Icon(Icons.tab_sharp)),
        IconButton(
            onPressed: () {
              //ChatHub().disconnect();
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
      BuildContext context, SearchAppBarDelegate searchDelegate) async {
    final String? selected = await showSearch<String>(
      context: context,
      delegate: searchDelegate,
    );

    print(selected);
  }
}
