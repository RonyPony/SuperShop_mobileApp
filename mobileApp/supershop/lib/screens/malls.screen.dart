import 'package:flutter/material.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

import 'cart.screen.dart';

class MallsScreen extends StatefulWidget {
  MallsScreen({Key key}) : super(key: key);
  static String routeName = "/mallsScreen";

  @override
  State<MallsScreen> createState() => _MallsScreenState();
}

class _MallsScreenState extends State<MallsScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: SideMenuDrawer(),
      appBar: AppBar(
        title: Text('Malls'),
        actions: [
           GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, CartScreen.routeName);
              },
            child: Icon(Icons.shopping_cart_rounded))
        ],
      ),
    );
  }
}