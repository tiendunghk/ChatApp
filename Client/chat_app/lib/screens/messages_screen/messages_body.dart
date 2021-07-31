import 'package:chat_app/models/chart_message.dart';
import 'package:chat_app/screens/messages_screen/chat_input/chat_input.dart';
import 'package:chat_app/screens/messages_screen/message.dart/message.dart';
import 'package:flutter/material.dart';

class MessagesBody extends StatelessWidget {
  const MessagesBody({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Expanded(
            child: Padding(
                padding: EdgeInsets.symmetric(horizontal: 8),
                child: ListView.builder(
                    itemBuilder: (_, index) {
                      return Message(message: demoChatMessages[index]);
                    },
                    itemCount: demoChatMessages.length))),
        ChatInput(),
      ],
    );
  }
}
