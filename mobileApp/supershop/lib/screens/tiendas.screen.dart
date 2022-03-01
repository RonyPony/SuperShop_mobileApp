import 'package:flutter/material.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

import 'cart.screen.dart';

class TiendasScreen extends StatefulWidget {
  TiendasScreen({Key key}) : super(key: key);
  static String routeName = "tiendasScreen";

  @override
  State<TiendasScreen> createState() => _TiendasScreenState();
}

class _TiendasScreenState extends State<TiendasScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: SideMenuDrawer(),
      appBar: AppBar(
        title: Text('Tiendas'),
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