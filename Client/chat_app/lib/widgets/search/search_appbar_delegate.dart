import 'package:chat_app/providers/user_provider.dart';
import 'package:chat_app/screens/messages_screen/messages_screen.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

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
                            Navigator.of(context)
                                .pushNamed(MessagesScreen.routeName);
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
    return response.length <= 0
        ? Center(
            child: Text('No data'),
          )
        : Container();
  }
}
