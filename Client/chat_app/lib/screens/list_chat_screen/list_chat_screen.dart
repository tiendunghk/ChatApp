import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/screens/list_chat_screen/list_chat_body.dart';
import 'package:chat_app/signalr/chatHub.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class ListChatScreen extends StatefulWidget {
  static const routeName = "/chat-list";
  const ListChatScreen({Key? key}) : super(key: key);

  @override
  _ListChatScreenState createState() => _ListChatScreenState();
}

class _ListChatScreenState extends State<ListChatScreen> {
  @override
  void initState() {
    super.initState();
    print('intit');
    connectSignalR();
  }

  void connectSignalR() async {
    //ChatHub().connectHub();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: buildAppBar(),
      body: ListChatBody(),
      floatingActionButton: FloatingActionButton(
        child: Icon(Icons.chat_bubble, color: Colors.white),
        backgroundColor: Colors.blueAccent,
        onPressed: () {},
        tooltip: "New Chat",
      ),
    );
  }

  AppBar buildAppBar() {
    return AppBar(
      title: Text("Chats"),
      automaticallyImplyLeading: false,
      actions: [
        IconButton(
            onPressed: () {
              Navigator.of(context).pushNamed('/');
            },
            icon: Icon(Icons.logout)),
        IconButton(
          onPressed: () {},
          icon: Icon(Icons.search),
        )
      ],
    );
  }
}
