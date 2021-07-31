import 'package:chat_app/constants.dart';
import 'package:chat_app/models/chart_message.dart';
import 'package:flutter/material.dart';

class Message extends StatelessWidget {
  final ChatMessage message;
  const Message({Key? key, required this.message}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment:
          message.isSender ? MainAxisAlignment.end : MainAxisAlignment.start,
      children: [
        Container(
            margin: EdgeInsets.only(top: defaultPadding),
            padding: EdgeInsets.symmetric(
              horizontal: defaultPadding * 0.75,
              vertical: defaultPadding / 2,
            ),
            decoration: BoxDecoration(
                color: message.isSender
                    ? Color(0xFF2B5278)
                    : Color(0xFF2B5278).withOpacity(0.5),
                borderRadius: BorderRadius.circular(20)),
            child: Text(message.text))
      ],
    );
  }
}
