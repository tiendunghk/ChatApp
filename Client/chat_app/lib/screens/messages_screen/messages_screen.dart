import 'package:chat_app/screens/messages_screen/messages_body.dart';
import 'package:flutter/material.dart';

class MessagesScreen extends StatelessWidget {
  const MessagesScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: buildAppBar(),
      body: MessagesBody(),
    );
  }

  AppBar buildAppBar() {
    return AppBar(
      title: Row(
        children: [
          CircleAvatar(
            backgroundImage: AssetImage("assets/images/user_2.png"),
          ),
          SizedBox(width: 8),
          Container(
            width: 120,
            child: Text(
              "Nguyen Tien Dung",
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
