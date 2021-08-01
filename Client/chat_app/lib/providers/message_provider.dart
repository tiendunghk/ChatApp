import 'dart:convert';

import 'package:chat_app/models/api_response.dart';
import 'package:chat_app/models/message.dart';
import 'package:chat_app/models/pagination_wrap.dart';
import 'package:flutter/foundation.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;

class MessageProvider with ChangeNotifier {
  String? _currentGroup;

  String? get currentGroup => _currentGroup;
  List<Message> _messages = [];
  List<Message> get messages => _messages;

  Future<void> getMessages(String groupId, int skipItems) async {
    _currentGroup = groupId;
    SharedPreferences sharePrf = await SharedPreferences.getInstance();
    final token = sharePrf.getString('access_token');
    final userId = sharePrf.getString('uid');

    http.Response response = await http.get(
      Uri.parse(
          'http://10.0.3.2:9999/api/message?groupId=$groupId&userId=$userId&pageSize=12&skipItems=$skipItems'),
      headers: {'Authorization': 'Bearer $token'},
    );

    if (response.statusCode == 200) {
      var jsonResponse = ApiResponse.fromJson(jsonDecode(response.body));

      var pagingWrapItems = PaginationWrap.fromJson(jsonResponse.data).items;

      var messagesData = List<Message>.from(
          pagingWrapItems.map((model) => Message.fromJson(model)));

      if (skipItems == 0) {
        _messages = messagesData;
      } else {
        var _cloneMessage = [..._messages];
        messagesData.addAll(_cloneMessage);
        _messages = messagesData;
      }

      notifyListeners();
    }
  }

  void newMessage(Message newMessage) {
    print('currentGroup: $_currentGroup');
    print('incomingGroup: ${newMessage.messageGroupChatId}');
    if (_currentGroup == newMessage.messageGroupChatId) {
      var _cloneMessage = [..._messages];
      _cloneMessage.add(newMessage);
      _messages = _cloneMessage;
      notifyListeners();
    }
  }
}
