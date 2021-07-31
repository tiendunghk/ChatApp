import 'dart:convert';

import 'package:chat_app/models/user.dart';
import 'package:chat_app/models/user_response.dart';
import 'package:chat_app/repository/api_helper.dart';
import 'package:chat_app/repository/my_api_helper.dart';
import 'package:dio/dio.dart';
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
    var api = MyApiHelper();
    var res = await api.postHTTP('/account/authenticate',
        UserRequest(email: email, fullName: name).toJson());

    if (res != null) {
      var userResponse = UserResponse.fromJson(res.data['data']);
      SharedPreferences shared = await SharedPreferences.getInstance();
      shared.setString('access_token', userResponse.jwToken);
      print(userResponse.id);
    }
    //var res1=await Dio().get('https://jsonplaceholder.typicode.com/todos/1');

    /*var url = Uri.parse('http://10.0.3.2:9999/api/account/authenticate');

    http.Response response = await http.post(url,
        body: json.encode({'email': email, 'fullName': name}),
        headers: {"Content-Type": "application/json"});

    print('zzzzzzzz :  ${response.body}');
    print('zzzzzzzz :  ${response.statusCode}');
    print('zzzzzzzz :  ${response.reasonPhrase}');*/
  }

  void _testJWT() async {
    try {
      //var res = await MyApiHelper().getHTTP('/test');
      /*final token =
          "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0ZjZmNGFjZS1iZDgyLTQ2ODYtYjRhYS1mYjNmZjk5YWJlODgiLCJ1aWQiOiI2YzI0MzZkNy0yOTM1LTRhMWUtYjdhNi1iZWM0MTUwN2M1MWMiLCJleHAiOjE2ODc3NTExMDksImlzcyI6IkNvcmVJZGVudGl0eSIsImF1ZCI6IkNvcmVJZGVudGl0eVVzZXIifQ.XaFFkR-CcbpfCtm6PkGwK-foDml-vS-79EdtvwCXS6w";

      var res = await Dio().get('http://10.0.3.2:9999/api/test',
          options: Options(headers: {"Authorization": "Bearer $token"}));
      print('res $res');*/

      var res = await MyApiHelper().getHTTP('/test');
      print('zzzzzzzz   $res');
    } on DioError catch (err) {
      print('loix $err');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: Text('Welcome to Simple Chat'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(30.0),
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
              Container(
                width: double.infinity,
                child: ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    primary: Colors.blue,
                  ),
                  onPressed: _testJWT,
                  child: Text(
                    "Test jwt",
                    style: TextStyle(color: Colors.white),
                  ),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
