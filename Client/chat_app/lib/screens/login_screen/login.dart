import 'dart:convert';

import 'package:chat_app/constants.dart';
import 'package:chat_app/models/api_response.dart';
import 'package:chat_app/models/user_response.dart';
import 'package:chat_app/screens/list_chat_screen/list_chat_screen.dart';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;

class Login extends StatefulWidget {
  const Login({Key? key}) : super(key: key);

  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  var _emailController = TextEditingController();
  var _nameController = TextEditingController();

  @override
  void dispose() {
    super.dispose();
    _emailController.dispose();
    _nameController.dispose();
  }

  bool _validateEmail(String value) {
    String pattern =
        r"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]"
        r"{0,253}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]"
        r"{0,253}[a-zA-Z0-9])?)*$";
    RegExp regex = new RegExp(pattern);
    if (!regex.hasMatch(value)) return false;

    return true;
  }

  void _login() async {
    var email = _emailController.text;
    var name = _nameController.text;
    if (email.isEmpty || name.isEmpty) {
      showDialog(
          context: context,
          builder: (ctx) {
            return AlertDialog(
              title: Text("Data empty"),
              content: Text("Please type data"),
              actions: [
                TextButton(
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                    child: Text('OK'))
              ],
            );
          });

      return;
    }

    if (!_validateEmail(_emailController.text)) {
      showDialog(
          context: context,
          builder: (ctx) {
            return AlertDialog(
              title: Text("Error"),
              content: Text("Please type normal email"),
              actions: [
                TextButton(
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                    child: Text('OK'))
              ],
            );
          });
      return;
    }

    print('call api');

    try {
      http.Response response =
          await http.post(Uri.parse('$baseURL/api/account/authenticate'),
              body: json.encode({
                'email': email,
                'fullName': name,
              }),
              headers: {'Content-Type': 'application/json'},
              encoding: Encoding.getByName("utf-8"));

      print('status code la: ${response.statusCode}');

      if (response.statusCode == 200) {
        var jsonResponse = ApiResponse.fromJson(jsonDecode(response.body));

        var userResponse = UserResponse.fromJson(jsonResponse.data);
        print(userResponse.id);
        SharedPreferences sharePrf = await SharedPreferences.getInstance();
        sharePrf.setString('uid', userResponse.id);
        sharePrf.setString("access_token", userResponse.jwToken);

        Navigator.of(context).pushReplacementNamed(ListChatScreen.routeName);
      }
    } on Exception catch (ex) {
      showDialog(
          context: context,
          builder: (ctx) {
            return AlertDialog(
              title: Text('Lỗi'),
              content: Text(ex.toString()),
              actions: [
                TextButton(
                    onPressed: () {
                      Navigator.pop(ctx);
                    },
                    child: Text('OK'))
              ],
            );
          });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: Text('Chat app'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(30.0),
        child: SingleChildScrollView(
          child: Center(
            child: Column(
              children: [
                TextField(
                  controller: _emailController,
                  decoration: InputDecoration(
                    hintText: "Type your email",
                    filled: true,
                  ),
                ),
                SizedBox(
                  height: 30,
                ),
                TextField(
                  controller: _nameController,
                  decoration: InputDecoration(
                    hintText: "Type your name",
                    filled: true,
                  ),
                ),
                SizedBox(
                  height: 30,
                ),
                Container(
                  width: double.infinity,
                  child: ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      primary: Colors.blue,
                    ),
                    onPressed: _login,
                    child: Text(
                      "Login",
                      style: TextStyle(color: Colors.white),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
