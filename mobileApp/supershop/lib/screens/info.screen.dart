import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:supershop/screens/home.screen.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

class InfoScreen extends StatefulWidget {
  InfoScreen({Key key}) : super(key: key);
  static String routeName = "infoScreen";
  @override
  State<InfoScreen> createState() => _InfoScreenState();
}

class _InfoScreenState extends State<InfoScreen> {
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    return Scaffold(
      backgroundColor: Colors.blue,
      appBar: AppBar(
        elevation: 0,
        actions: [
          GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, HomeScreen.routeName);
              },
              child: Icon(Icons.arrow_back_ios))
        ],
      ),
      drawer: SideMenuDrawer(),
      body: SingleChildScrollView(
        // controller: controller,
        child: Center(
          child: Column(
            children: [
              Padding(
                padding: EdgeInsets.only(top: screenSize.height*0.11),
                child: Text(
                  'Version 0.1.2',
                  style: TextStyle(color: Colors.white, fontSize: 19),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 30),
                child: SvgPicture.asset(
                  "assets/logo-sin-nombre.svg",
                  color: Colors.white,
                  height: 130,
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 10),
                child: Text(
                  'SuperShop',
                  style: TextStyle(
                      fontSize: 40,
                      fontWeight: FontWeight.bold,
                      color: Colors.white),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 10),
                child: Text('2022 SuperShop inc.',style: TextStyle(color: Colors.white,fontWeight: FontWeight.bold,fontSize: 16),),
              )
            ],
          ),
        ),
      ),
    );
  }
}
