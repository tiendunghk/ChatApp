import 'dart:convert';

import 'package:chat_app/providers/message_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;

import '../../../constants.dart';

class ChatInput extends StatelessWidget {
  const ChatInput({
    Key? key,
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
        if (response.statusCode == 200) {}
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
                  horizontal: defaultPadding * 0.75,
                ),
                decoration: BoxDecoration(
                  color: primaryColor.withOpacity(0.05),
                  borderRadius: BorderRadius.circular(40),
                ),
                child: Row(
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
                    IconButton(
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
