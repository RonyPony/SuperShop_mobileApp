import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:supershop/constants.dart';
import 'package:supershop/contracts/auth_service.contract.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/providers/authProvider.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/routes.dart';
import 'package:supershop/screens/authentication/login.screen.dart';
import 'package:supershop/services/auth.service.dart';
import 'package:supershop/services/product.service.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    AuthServiceContract _contrato;
    ProductServiceContract prodCont;
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (context)=>AuthProvider(AuthenticationService())),
        ChangeNotifierProvider(create: (context)=>ProductProvider(ProductService()))
      ],
      child:
    
     MaterialApp(
      title: 'Flutter Demo',
      
      routes: routes,
      theme: ThemeData(        
        primarySwatch:kPrimaryColor,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: LoginScreen(),
    ));
  }
}
