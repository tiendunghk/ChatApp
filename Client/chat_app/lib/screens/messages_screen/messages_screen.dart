import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/screens/messages_screen/messages_body.dart';
import 'package:flutter/material.dart';

class MessagesScreen extends StatefulWidget {
  static const routeName = "/messages-screen";
  const MessagesScreen({Key? key}) : super(key: key);

  @override
  _MessagesScreenState createState() => _MessagesScreenState();
}

class _MessagesScreenState extends State<MessagesScreen> {
  var _isInit = true;
  late GroupChatModel _groupChat;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: buildAppBar(),
      body: MessagesBody(groupId: _groupChat.groupChatId),
    );
  }

  @override
  void didChangeDependencies() {
    if (_isInit) {
      final groupChat =
          ModalRoute.of(context)!.settings.arguments as GroupChatModel;
      setState(() {
        _groupChat = groupChat;
      });
    }

    _isInit = false;
    super.didChangeDependencies();
  }

  AppBar buildAppBar() {
    return AppBar(
      title: Row(
        children: [
          CircleAvatar(
            backgroundImage: NetworkImage(_groupChat.groupAvatar),
          ),
          SizedBox(width: 8),
          Container(
            width: 120,
            child: Text(
              _groupChat.groupChatName,
              maxLines: 3,
              overflow: TextOverflow.ellipsis,
              style: TextStyle(fontSize: 12),
            ),
          )
        ],
      ),
    );
  }
}
