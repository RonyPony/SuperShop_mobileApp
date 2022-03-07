import 'package:flutter/material.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

import 'cart.screen.dart';
import 'home.screen.dart';

class ShoppingDetailScreen extends StatefulWidget {
  ShoppingDetailScreen({Key key}) : super(key: key);
  static String routeName = '/shoppingDetailsScreen';
  @override
  State<ShoppingDetailScreen> createState() => _ShoppingDetailScreenState();
}

class _ShoppingDetailScreenState extends State<ShoppingDetailScreen> {
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    return Scaffold(
      drawer: SideMenuDrawer(),
      appBar: AppBar(
        actions: [
          GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, CartScreen.routeName);
              },
              child: Icon(Icons.shopping_cart_rounded)),
          SizedBox(
            width: 20,
          ),
          GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, HomeScreen.routeName);
              },
              child: Icon(Icons.arrow_back_ios))
        ],
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.only(top: 50),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [Text('Datos de envio')],
            ),
          ),
          Padding(
            padding: EdgeInsets.only(right: screenSize.width * 0.7, top: 20),
            child: Text('Mis lugares'),
          ),
          _buildMyAddresses(),
        ],
      ),
    );
  }

  _buildMyAddresses() {
    return TextField(
      decoration: InputDecoration(
        hintText: "Mi casa",
        fillColor: Colors.red
      ),
    );
  }
}
