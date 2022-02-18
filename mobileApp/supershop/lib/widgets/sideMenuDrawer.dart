import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class SideMenuDrawer extends StatelessWidget {
  const SideMenuDrawer({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    return Drawer(
        child: SafeArea(
      child: ListView(
        padding: EdgeInsets.zero,
        children: [
          Container(
            height: screenSize.height * 0.07,
            child: DrawerHeader(
                child: Text('SuperShop',
                    style: TextStyle(color: Colors.white, fontSize: 25)),
                decoration: BoxDecoration(color: Colors.blue),
                margin: EdgeInsets.all(0.0),
                padding: EdgeInsets.only(
                    top: screenSize.height * 0.03,
                    left: screenSize.width * 0.2)),
          ),
          ListTile(
            leading: Icon(
              CupertinoIcons.house_fill,
              color: Colors.blue,
            ),
            title: Text(
              "Inicio",
              style: TextStyle(color: Colors.blue, fontSize: 18),
            ),
            onTap: () {
              //TODO
            },
          ),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 15.0),
            child: Container(
              height: 1.5,
              color: Colors.blue,
            ),
          ),
          ListTile(
            leading: Icon(
              CupertinoIcons.person_fill,
              color: Colors.blue,
            ),
            title: Text(
              "Perfil",
              style: TextStyle(color: Colors.blue, fontSize: 18),
            ),
            onTap: () {
              //TODO
            },
          ),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 15.0),
            child: Container(
              height: 1.5,
              color: Colors.blue,
            ),
          ),
          ListTile(
            leading: Icon(
              CupertinoIcons.gear_alt_fill,
              color: Colors.blue,
            ),
            title: Text(
              "Configuracion",
              style: TextStyle(color: Colors.blue, fontSize: 18),
            ),
            onTap: () {
              //TODO
            },
          ),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 15.0),
            child: Container(
              height: 1.5,
              color: Colors.blue,
            ),
          ),
          Padding(
            padding: EdgeInsets.only(
                top: screenSize.height * 0.6, right: screenSize.width * 0.4),
            child: TextButton(
              onPressed: () {},
              child: Text('Cerrar Sesion'),
            ),
          )
        ],
      ),
    ));
  }
}
