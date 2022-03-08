import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:provider/provider.dart';
import 'package:supershop/constants.dart';
import 'package:supershop/models/mall.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/cart.screen.dart';
import 'package:supershop/screens/storeDetails.screen.dart';
import 'package:supershop/screens/stores.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';
import 'package:supershop/widgets/storeLogo.dart';

class HomeScreen extends StatefulWidget {
  HomeScreen({Key key}) : super(key: key);
  static String routeName = "/homeScreen";

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    final _prodProvider = Provider.of<ProductProvider>(context, listen: false);

    Future<List<Mall>> _futuro = _prodProvider.getMalls();

    return Scaffold(
        drawer: SideMenuDrawer(),
        appBar: AppBar(
          backgroundColor: Colors.blue,
          actions: [
            GestureDetector(
                onTap: () {
                  Navigator.pushNamed(context, CartScreen.routeName);
                },
                child: Icon(Icons.shopping_cart_rounded))
          ],
        ),
        body: Column(
          children: [
            Padding(
              padding: EdgeInsets.only(top: screenSize.height * 0.06),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(
                    "Donde quieres comprar ?",
                    style: TextStyle(
                        fontSize: 18,
                        color: Colors.blue,
                        fontWeight: FontWeight.bold),
                  )
                ],
              ),
            ),
            SizedBox(
              height: 20,
            ),
            FutureBuilder<List<Mall>>(
              future: _futuro,
              // initialData: InitialData,
              builder:
                  (BuildContext context, AsyncSnapshot<List<Mall>> snapshot) {
                if (snapshot.connectionState == ConnectionState.done) {
                  if (snapshot.hasError) {
                    return Text('Error');
                  }

                  if (snapshot.hasData) {
                    return Container(
                      height: screenSize.height * 0.7,
                      child: ListView.builder(
                        itemCount: snapshot.data.length,
                        itemBuilder: (BuildContext context, int index) {
                          return GestureDetector(
                            onTap: () {
                              Navigator.pushNamed(context, StoresScreen.routeName,arguments: snapshot.data[index]);
                            },
                            child: StoreLogo(
                              storeName: snapshot.data[index].name,
                              storeLogo: snapshot.data[index].imageUrl,
                            ),
                          );
                        },
                      ),
                    );
                    // return Text(snapshot.data.length.toString());
                  }
                } else {
                  return Center(child: CircularProgressIndicator());
                }
              },
            ),
            // StoreLogo(
            //   onTap: () {
            //     Navigator.pushNamed(context, StoresScreen.routeName);
            //     //  Navigator.pushAndRemoveUntil(
            //     //           context,
            //     //           MaterialPageRoute(builder: (BuildContext context) => StoresScreen()),
            //     //           ModalRoute.withName(StoresScreen.routeName)
            //     //       );
            //   },
            //   storeName: "Megacentro",
            //   storeLogo: "assets/logos/mega.png",
            // ),
            // SizedBox(
            //   height: 20,
            // ),
            // StoreLogo(
            //   storeName: "Sambil",
            //   storeLogo: "assets/logos/sambil.png",
            // ),
          ],
        ));
  }
}
