import 'dart:convert';

import 'package:chat_app/models/api_response.dart';
import 'package:chat_app/models/user_search_reponse.dart';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class UserProvider with ChangeNotifier {
  List<UserSearchResponse> _userSearch = [];

  List<UserSearchResponse> get userSearch {
    return _userSearch;
  }

  Future<void> searchUser(String keyWord) async {
    SharedPreferences sharePrf = await SharedPreferences.getInstance();
    final userId = sharePrf.getString('uid');
    final token = sharePrf.getString('access_token');

    http.Response response = await http.get(
      Uri.parse(
          'http://10.0.3.2:9999/api/user/search?userId=$userId&keyword=$keyWord'),
      headers: {'Authorization': 'Bearer $token'},
    );

    if (response.statusCode == 200) {
      var jsonResponse = ApiResponse.fromJson(jsonDecode(response.body));

      var dataReponse = List<UserSearchResponse>.from(
          jsonResponse.data.map((model) => UserSearchResponse.fromJson(model)));
      _userSearch = dataReponse;
      notifyListeners();
    }
  }
}
