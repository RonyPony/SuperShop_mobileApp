import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/mall.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/storeDetails.screen.dart';
import 'package:supershop/screens/stores.dart';
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
    final _prodProvider = Provider.of<ProductProvider>(context, listen: false);
    Future<List<Mall>> _futuro = _prodProvider.getMalls();
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
      body: FutureBuilder<List<Mall>>(
        future: _futuro,
        // initialData: InitialData,
        builder: (BuildContext context, AsyncSnapshot<List<Mall>> snapshot) {
          if (snapshot.connectionState == ConnectionState.done) {
            if (snapshot.hasError) {
              return Text('Error');
            }

            if (snapshot.hasData) {
              return ListView.builder(
                itemCount: snapshot.data.length,
                itemBuilder: (BuildContext context, int index) {
                  return buildMall(snapshot.data[index]);
                },
              );
              // return Text(snapshot.data.length.toString());
            }
          } else {
            return Center(child: CircularProgressIndicator());
          }
        },
      ),
    );
  }

  Widget buildMall(Mall mall) {
    Size screenSize = MediaQuery.of(context).size;
    return Padding(
        padding: const EdgeInsets.only(top: 15, bottom: 15, left: 5, right: 5),
        child: Column(
          children: [
            GestureDetector(
              onTap: (){
                Navigator.pushNamed(context, StoresScreen.routeName,arguments: mall);
              },
                            child: Container(
                padding: EdgeInsets.only(bottom: 15),
                decoration: BoxDecoration(
                    color: Colors.red,
                    borderRadius: BorderRadius.only(
                        topRight: Radius.circular(50),
                        topLeft: Radius.circular(50),
                        bottomLeft: Radius.circular(10),
                        bottomRight: Radius.circular(10))),
                child: Container(
                  height: screenSize.height * 0.4,
                  width: screenSize.width,
                  child: mall.imageUrl!=null?Image.network(mall.imageUrl):SizedBox(),
                  decoration: BoxDecoration(
                      gradient: LinearGradient(
                          begin: Alignment.topRight,
                          end: Alignment.bottomLeft,
                          colors: [Colors.pink, Colors.blue]),
                      color: Colors.blue,
                      shape: BoxShape.rectangle,
                      borderRadius: BorderRadius.only(
                          topRight: Radius.circular(15.0),
                          bottomLeft: Radius.circular(15.0),
                          topLeft: Radius.circular(15.0),
                          bottomRight: Radius.circular(15.0))),
                ),
              ),
            ),
            Text(
              mall.name.toString(),
              style: TextStyle(fontSize: 20),
            )
          ],
        ));
  }
}
