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
            margin: EdgeInsets.only(top: kDefaultPadding),
            padding: EdgeInsets.symmetric(
              horizontal: kDefaultPadding * 0.75,
              vertical: kDefaultPadding / 2,
            ),
            decoration: BoxDecoration(
                color: kPrimaryColor, borderRadius: BorderRadius.circular(20)),
            child: Text(message.text))
      ],
    );
  }
}
