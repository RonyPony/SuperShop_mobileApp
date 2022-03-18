import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

import 'cart.screen.dart';
import 'home.screen.dart';

class ConfirmScreen extends StatefulWidget {
  ConfirmScreen({Key key}) : super(key: key);
  static String routeName = '/confirmScreen';

  @override
  State<ConfirmScreen> createState() => _ConfirmScreenState();
}

class _ConfirmScreenState extends State<ConfirmScreen> {
  @override
  Widget build(BuildContext context) {
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
      body: SingleChildScrollView(
        // controller: controller,
        child: Padding(
          padding: const EdgeInsets.only(top: 100),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Container(
                decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(20), 
                  
                    color: Colors.grey.withOpacity(.5)),
                child: Column(
                  children: [
                    SizedBox(height: 50,),
                    SvgPicture.asset(
                      'assets/pulgar.svg',
                      height: 80,
                      color: Colors.blue,
                    ),
                    Text('Hemos Recibido Tu Pedido!',style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold
                    ),),
                    Text('Pronto te lo haremos llegar',style: TextStyle(
                      fontSize: 16
                    ),),
                    SizedBox(height: 50,),
                    ElevatedButton(
                      style: ButtonStyle(
                          backgroundColor:
                              MaterialStateProperty.all(Colors.blue),
                          shape:
                              MaterialStateProperty.all<RoundedRectangleBorder>(
                                  RoundedRectangleBorder(
                                      borderRadius: BorderRadius.circular(50),
                                      side: BorderSide(color: Colors.black)))),
                      child: Container(
                        width: 100,
                        child: const Text(
                          'Ir a Inicio',
                          textAlign: TextAlign.center,
                          style: TextStyle(
                              fontSize: 22,
                              fontWeight: FontWeight.bold,
                              color: Colors.white),
                        ),
                      ),
                      onPressed: () async {
                        Navigator.pushNamed(context, HomeScreen.routeName);
                      },
                    )
                  ],
                ),
                height: 300,
                width: 300,
              )
            ],
          ),
        ),
      ),
    );
  }
}
