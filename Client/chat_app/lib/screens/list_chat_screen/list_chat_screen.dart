import 'package:chat_app/constants.dart';
import 'package:chat_app/screens/list_chat_screen/list_chat_body.dart';
import 'package:flutter/material.dart';

class ListChatScreen extends StatelessWidget {
  const ListChatScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: buildAppBar(),
      body: ListChatBody(),
      floatingActionButton: FloatingActionButton(
        child: Icon(Icons.chat_bubble, color: Colors.white),
        backgroundColor: kPrimaryColor,
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
          onPressed: () {},
          icon: Icon(Icons.search),
        )
      ],
    );
  }
}
