import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/malls.model.dart';
import 'package:supershop/providers/productProvider.dart';
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
    final _prodProvider = Provider.of<ProductProvider>(context,listen:false);
    Future<List<Malls>>_futuro=_prodProvider.getMalls();
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
      body: FutureBuilder<List<Malls>>(
        future: _futuro,
        // initialData: InitialData,
        builder: (BuildContext context, AsyncSnapshot snapshot) {
          return ListView.builder(
            itemCount: snapshot.data,
            itemBuilder: (BuildContext context, int index) {
              return ;
            },
          );
        },
      ),
    );
  }
}