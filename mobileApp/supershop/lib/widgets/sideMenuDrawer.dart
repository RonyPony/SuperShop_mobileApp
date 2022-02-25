import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:supershop/screens/home.screen.dart';
import 'package:supershop/screens/authentication/login.screen.dart';

class SideMenuDrawer extends StatelessWidget {
  const SideMenuDrawer({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    return Drawer(
      // semanticLabel: 'sss',
      elevation: 0,
      child: Column(
        children: [
          Container(
            height: screenSize.height * 0.9,
            child: ListView(
              padding: EdgeInsets.zero,
              children: [
                Container(
                  height: screenSize.height * 0.13,
                  child: DrawerHeader(
                    // padding: EdgeInsets.only(right: screenSize.width*0.1),
                    child: Row(
                      children: [
                        GestureDetector(
                          onTap: () {
                            Navigator.pop(context);
                          },
                          child: Icon(
                            Icons.close,
                            color: Colors.white,
                            size: 35,
                          ),
                        ),
                      ],
                    ),
                    // child: Text(
                    //   "X",
                    //   style: TextStyle(
                    //     color: Colors.white,
                    //     fontSize: 30
                    //   ),
                    // ),
                    decoration: BoxDecoration(color: Colors.blue),
                  ),
                ),
                _createMenuItem("Inicio", "assets/casita.svg",
                    HomeScreen.routeName, context),
                _createSplitter(),
                _createMenuItem(
                    "Malls", "assets/malls-icon.svg", "routename", context),
                _createSplitter(),
                _createMenuItem(
                    "Tiendas", "assets/tienda-icon.svg", "routename", context),
                _createSplitter(),
                _createMenuItem(
                    "Perfil", "assets/user.svg", "routename", context),
                _createSplitter(),
                _createMenuItem("Informacion", "assets/info-icon.svg",
                    "routename", context),
                _createSplitter(),
              ],
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(left: 20),
            child: GestureDetector(
              onTap: () {
                Navigator.pushNamedAndRemoveUntil(context, LoginScreen.routeName, (route) => false);
              },
              child: Row(
                children: [
                  Text(
                    'Cerrar Sesion',
                    style: TextStyle(color: Colors.blue),
                  ),
                ],
              ),
            ),
          )
        ],
      ),
    );
  }

  _createMenuItem(
      String label, String iconRoute, String route, BuildContext context) {
    return Container(
      height: 50,
      child: ListTile(
        leading: SvgPicture.asset(
          iconRoute,
          color: Colors.blue,
          height: 24,
        ),
        // leading: Icon(CupertinoIcons.house_fill,color: Colors.blue,),
        title: Text(
          label,
          style: TextStyle(color: Colors.blue, fontSize: 18),
        ),
        onTap: () {
          Navigator.pushNamedAndRemoveUntil(context, route, (route) => false);
        },
      ),
    );
  }

  _createSplitter() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.0),
      child: Container(
        height: 1.0,
        color: Colors.blue,
      ),
    );
  }
}
