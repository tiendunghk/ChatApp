import 'dart:convert';
import 'package:chat_app/models/api_response.dart';
import 'package:chat_app/models/message.dart';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class GroupChatModel {
  String groupChatId;
  String groupChatName;
  DateTime groupChatUpdatedAt;
  String groupAvatar;
  String? lastestMes;

  GroupChatModel(
      {required this.groupChatId,
      required this.groupChatName,
      required this.groupChatUpdatedAt,
      required this.groupAvatar,
      this.lastestMes});

  factory GroupChatModel.fromJson(Map<String, dynamic> json) => GroupChatModel(
      groupChatId: json['groupChatId'],
      groupChatName: json['groupChatName'],
      groupChatUpdatedAt: DateTime.parse(json['groupChatUpdatedAt']),
      groupAvatar: json['groupAvatar'],
      lastestMes: json['lastestMes']);

  @override
  String toString() {
    return '${this.groupChatId}   -   ${this.groupChatName} -   ${this.groupChatUpdatedAt}';
  }
}

class GroupChat with ChangeNotifier {
  List<GroupChatModel> _items = [];

  List<GroupChatModel> get items {
    return _items;
  }

  Future<void> getGroupChat() async {
    SharedPreferences sharePrf = await SharedPreferences.getInstance();
    final userId = sharePrf.getString('uid');
    final token = sharePrf.getString('access_token');

    http.Response response = await http.get(
      Uri.parse('http://10.0.3.2:9999/api/groupchat/user/$userId'),
      headers: {'Authorization': 'Bearer $token'},
    );

    if (response.statusCode == 200) {
      var jsonResponse = ApiResponse.fromJson(jsonDecode(response.body));

      var dataReponse = List<GroupChatModel>.from(
          jsonResponse.data.map((model) => GroupChatModel.fromJson(model)));
      _items = dataReponse;
      notifyListeners();
    }
  }

  void newMessage(Message newMessage) {
    var cloneGrChat = [..._items];
    var findGrIndex = cloneGrChat.indexWhere(
        (element) => element.groupChatId == newMessage.messageGroupChatId);
    if (findGrIndex >= 0) {
      var currentObj = cloneGrChat[findGrIndex];
      currentObj.groupChatUpdatedAt = newMessage.messageCreatedAt;
      currentObj.lastestMes = newMessage.messageContent;
      cloneGrChat.removeAt(findGrIndex);

      cloneGrChat.insert(0, currentObj);
    } else {
      var newGroup = GroupChatModel(
          groupChatId: DateTime.now().toString(),
          groupChatName: newMessage.messengerUserName,
          groupChatUpdatedAt: newMessage.messageCreatedAt,
          groupAvatar: newMessage.messengerUserAvatar,
          lastestMes: newMessage.messageContent);

      cloneGrChat.insert(0, newGroup);
    }

    _items = cloneGrChat;
    notifyListeners();
  }
}
