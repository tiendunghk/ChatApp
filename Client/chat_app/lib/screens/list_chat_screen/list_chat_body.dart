import 'package:chat_app/models/chat.dart';
import 'package:chat_app/screens/list_chat_screen/chat_list_item/chat_list_item.dart';
import 'package:flutter/material.dart';

class ListChatBody extends StatelessWidget {
  const ListChatBody({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Expanded(
            child: ListView.builder(
                itemBuilder: (ctx, index) {
                  return ChatListItem(chat: chatsData[index]);
                },
                itemCount: chatsData.length))
      ],
    );
  }
}
