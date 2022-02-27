import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:supershop/screens/productDetails.screen.dart';

class CartItem extends StatelessWidget {
  const CartItem(
      {Key key, this.productName, this.productPrice, this.productImage})
      : super(key: key);
  final productName;
  final productPrice;
  final productImage;
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(right: 30, left: 30, top: 10),
      child: Container(
        // padding: EdgeInsets.all(0),
        decoration: BoxDecoration(
            color: Colors.blue,
            borderRadius: BorderRadius.all(Radius.circular(25))),
        child: Row(
          children: [
            Padding(
                padding: const EdgeInsets.only(
                    top: 15, bottom: 15, left: 15, right: 15),
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(20),
                  child: Image.network(
                    productImage,
                    width: 80,
                    height: 70,
                  ),
                )),
            Column(
              // mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  productName,
                  style: TextStyle(fontSize: 20, color: Colors.white),
                ),
                Text(
                  "RD${productPrice}",
                  style: TextStyle(
                      fontSize: 15,
                      color: Colors.white,
                      fontWeight: FontWeight.bold),
                ),
              ],
            ),
            SizedBox(
              width: 40,
            ),
            Container(
                // color: Colors.white,
                decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(7)),
                child: Icon(
                  Icons.delete_forever_rounded,
                  color: Colors.red,
                  size: 30,
                )),
            SizedBox(
              width: 30,
            ),
            SvgPicture.asset(
              "assets/flechita-saltar.svg",
              height: 15,
              color: Colors.white,
            )
          ],
        ),
      ),
    );
  }
}
