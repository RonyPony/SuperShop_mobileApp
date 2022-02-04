import 'package:flutter/material.dart';

import '../../constants.dart';

class ForgottenPasswrodScreen extends StatefulWidget {
  const ForgottenPasswrodScreen({Key key}) : super(key: key);
  static String routeName = "/forgottenScreen";
  @override
  _ForgottenPasswrodScreenState createState() =>
      _ForgottenPasswrodScreenState();
}

class _ForgottenPasswrodScreenState extends State<ForgottenPasswrodScreen> {
  @override
  Widget build(BuildContext context) {
    TextEditingController correo;
    return Scaffold(
      appBar: AppBar(),
      body: Padding(
          padding: const EdgeInsets.all(10),
          child: ListView(
            children: <Widget>[
              Container(
                  alignment: Alignment.center,
                  padding: EdgeInsets.only(
                      top: MediaQuery.of(context).size.height * 0.2),
                  child: const Text(
                    appName,
                    style: TextStyle(
                        color: kPrimaryColor,
                        fontWeight: FontWeight.w500,
                        fontSize: 30),
                  )),
              Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child: const Text(
                    'Recuperar Contraseña',
                    style: TextStyle(fontSize: 20),
                  )),
              Container(
                padding: const EdgeInsets.all(10),
                child: TextField(
                  controller: correo,
                  decoration: const InputDecoration(
                    border: OutlineInputBorder(),
                    labelText: 'Correo Electronico',
                  ),
                ),
              ),
              SizedBox(
                height: MediaQuery.of(context).size.height * 0.1,
              ),
              Container(
                  height: 50,
                  padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                  child: ElevatedButton(
                    child: const Text('Acceder'),
                    onPressed: () {
                      print(correo.text);
                    },
                  )),
            ],
          )),
    );
  }
}
