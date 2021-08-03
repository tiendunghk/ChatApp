import 'package:chat_app/constants.dart';
import 'package:chat_app/models/message.dart' as mes;
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class Message extends StatelessWidget {
  final mes.Message message;
  const Message({Key? key, required this.message}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.of(context).size;
    return Row(
      mainAxisAlignment:
          message.isSender ? MainAxisAlignment.end : MainAxisAlignment.start,
      children: [
        Container(
          margin: EdgeInsets.only(top: defaultPadding),
          padding: EdgeInsets.symmetric(
              horizontal: defaultPadding * 0.4, vertical: defaultPadding / 4),
          decoration: BoxDecoration(
              color: message.isSender
                  ? Color(0xFF2B5278)
                  : Color(0xFF2B5278).withOpacity(0.5),
              borderRadius: BorderRadius.circular(15)),
          child: Container(
            constraints: BoxConstraints(
              maxWidth: size.width * 0.8,
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(message.messageContent),
                SizedBox(
                  height: 5,
                ),
                Text(
                  DateFormat('hh:MM a')
                      .format(message.messageCreatedAt.toLocal()),
                  style: TextStyle(
                      color: Colors.white.withOpacity(0.7), fontSize: 10),
                )
              ],
            ),
          ),
        )
      ],
    );
  }
}
