import 'dart:convert';

import 'package:chat_app/providers/message_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;

import '../../../constants.dart';

class ChatInput extends StatelessWidget {
  final Function scrollFunc;
  const ChatInput({
    Key? key,
    required this.scrollFunc,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var _inputController = TextEditingController();

    void _submit() async {
      final content = _inputController.text;
      if (content.isNotEmpty) {
        SharedPreferences sharePrf = await SharedPreferences.getInstance();
        final userId = sharePrf.getString('uid');
        final token = sharePrf.getString('access_token');
        http.Response response = await http.post(
            Uri.parse('$baseURL/api/chat/messages'),
            body: json.encode({
              'groupId': Provider.of<MessageProvider>(context, listen: false)
                  .currentGroup,
              'userId': userId,
              'message': content,
            }),
            headers: {
              'Content-Type': 'application/json',
              'Authorization': 'Bearer $token'
            },
            encoding: Encoding.getByName("utf-8"));
        if (response.statusCode == 200) {
          await Future.delayed(Duration(seconds: 0));
          scrollFunc();
        }
      }
    }

    return Container(
      padding: EdgeInsets.symmetric(
        horizontal: defaultPadding,
        vertical: defaultPadding / 2,
      ),
      decoration: BoxDecoration(
        color: Theme.of(context).scaffoldBackgroundColor,
        boxShadow: [
          BoxShadow(
            offset: Offset(0, 4),
            blurRadius: 32,
            color: Color(0xFF087949).withOpacity(0.08),
          ),
        ],
      ),
      child: SafeArea(
        child: Row(
          children: [
            Expanded(
              child: Container(
                padding: EdgeInsets.symmetric(
                  horizontal: defaultPadding * 0.4,
                ),
                decoration: BoxDecoration(
                  color: Color(0xFF2B5278).withOpacity(0.5),
                  borderRadius: BorderRadius.circular(40),
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Expanded(
                      child: TextField(
                        controller: _inputController,
                        decoration: InputDecoration(
                          hintText: "Type message",
                          border: InputBorder.none,
                        ),
                        onSubmitted: (value) {
                          //if (value.isNotEmpty) {}\
                          _submit();
                        },
                      ),
                    ),
                    SizedBox(width: 20),
                    Container(
                      margin: EdgeInsets.only(left: 50),
                      child: IconButton(
                        icon: Icon(
                          Icons.send,
                          color: Theme.of(context)
                              .textTheme
                              .bodyText1!
                              .color!
                              .withOpacity(0.64),
                        ),
                        onPressed: () {
                          _submit();
                        },
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
