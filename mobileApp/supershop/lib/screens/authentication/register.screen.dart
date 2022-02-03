import 'package:flutter/material.dart';

import '../../constants.dart';

class RegisterScreen extends StatefulWidget {
  RegisterScreen({Key key}) : super(key: key);
  static String routeName="/registerScreen";
  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar:AppBar(
        elevation: 0,
        toolbarHeight: 80,
        // backgroundColor: Colors.white,
        title: Text("Volver"),
      ),
      body: RegisterWidget(),
    );
  }
}

class RegisterWidget extends StatefulWidget {
  const RegisterWidget({Key key}) : super(key: key);
 
  @override
  State<RegisterWidget> createState() => _RegisterWidgetState();
}
 
class _RegisterWidgetState extends State<RegisterWidget> {
  TextEditingController nameController = TextEditingController();
  TextEditingController passwordController = TextEditingController();
 
  @override
  Widget build(BuildContext context) {
    return Padding(
        padding: const EdgeInsets.all(10),
        child: ListView(
          children: <Widget>[
            Container(
                alignment: Alignment.center,
                padding:  EdgeInsets.only(top: MediaQuery.of(context).size.height*0.1),
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
                  'Registrate',
                  style: TextStyle(fontSize: 20),
                )),
            Container(
              padding: const EdgeInsets.all(10),
              child: TextField(
                controller: nameController,
                decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  labelText: 'Correo Electronico',
                ),
              ),
            ),
            Container(
              padding: const EdgeInsets.fromLTRB(10, 10, 10, 0),
              child: TextField(
                obscureText: true,
                controller: passwordController,
                decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  labelText: 'Clave',
                ),
              ),
            ),
            SizedBox(
              height: 20,
            ),
            
            Container(
                height: 50,
                padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                child: ElevatedButton(
                  child: const Text('Registrar'),
                  onPressed: () {
                    print(nameController.text);
                    print(passwordController.text);
                  },
                )
            ),
          ],
        ));
  }
}