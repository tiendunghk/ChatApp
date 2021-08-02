import 'dart:convert';

import 'package:chat_app/constants.dart';
import 'package:chat_app/models/api_response.dart';
import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/providers/user_provider.dart';
import 'package:chat_app/screens/messages_screen/messages_screen.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class SearchAppBarDelegate extends SearchDelegate<String> {
  var response = [];
  SearchAppBarDelegate() {}

  // Setting leading icon for the search bar.
  //Clicking on back arrow will take control to main page
  @override
  Widget buildLeading(BuildContext context) {
    return IconButton(
      tooltip: 'Back',
      icon: AnimatedIcon(
        icon: AnimatedIcons.menu_arrow,
        progress: transitionAnimation,
      ),
      onPressed: () {
        //Take control back to previous page
        this.close(context, '');
      },
    );
  }

  // Builds page to populate search results.
  @override
  Widget buildResults(BuildContext context) {
    void goToChat(String userTwo) async {
      SharedPreferences sharePrf = await SharedPreferences.getInstance();
      final userId = sharePrf.getString('uid');
      final token = sharePrf.getString('access_token');
      http.Response response = await http.post(
          Uri.parse('$baseURL/api/groupchat'),
          body: json.encode({
            'userOneId': userId,
            'userTwoId': userTwo,
          }),
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer $token'
          },
          encoding: Encoding.getByName("utf-8"));

      if (response.statusCode == 200) {
        var jsonResponse = ApiResponse.fromJson(jsonDecode(response.body));

        var groupResponse = GroupChatModel.fromJson(jsonResponse.data);

        Navigator.of(context)
            .pushNamed(MessagesScreen.routeName, arguments: groupResponse);
      }
    }

    //this.query
    return FutureBuilder(
        future: Provider.of<UserProvider>(context, listen: false)
            .searchUser(this.query),
        builder: (ctx, snapshot) => snapshot.connectionState ==
                ConnectionState.waiting
            ? Center(
                child: CircularProgressIndicator(),
              )
            : Consumer<UserProvider>(
                child: Center(
                  child: Text("Not found!"),
                ),
                builder: (ctx, userProvider, child) {
                  if (userProvider.userSearch.length <= 0) return child!;

                  return ListView.builder(
                      itemBuilder: (_, index) {
                        return InkWell(
                          onTap: () {
                            print('on tap');
                            goToChat(userProvider.userSearch[index].userId);
                          },
                          child: Card(
                            child: ListTile(
                              leading: CircleAvatar(
                                backgroundImage: NetworkImage(userProvider
                                    .userSearch[index].userImageUrl),
                              ),
                              title: Text(
                                  userProvider.userSearch[index].userFullname),
                            ),
                          ),
                        );
                      },
                      itemCount: userProvider.userSearch.length);
                }));
  }

  @override
  List<Widget> buildActions(BuildContext context) {
    return <Widget>[];
  }

  @override
  Widget buildSuggestions(BuildContext context) {
    return Container();
  }
}
