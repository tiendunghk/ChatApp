import 'package:chat_app/constants.dart';
import 'package:chat_app/models/message.dart' as mes;
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class Message extends StatelessWidget {
  final mes.Message message;
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
              vertical: defaultPadding / 3,
            ),
            decoration: BoxDecoration(
                color: message.isSender
                    ? Color(0xFF2B5278)
                    : Color(0xFF2B5278).withOpacity(0.5),
                borderRadius: BorderRadius.circular(20)),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(message.messageContent),
                SizedBox(
                  width: 10,
                ),
                Container(
                  margin: const EdgeInsets.only(top: 12),
                  child: Text(
                    DateFormat('hh:MM a').format(message.messageCreatedAt),
                    style: TextStyle(
                        color: Colors.white.withOpacity(0.7), fontSize: 10),
                  ),
                ),
              ],
            ))
      ],
    );
  }
}
