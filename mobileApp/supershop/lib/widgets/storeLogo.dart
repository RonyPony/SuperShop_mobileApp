import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class StoreLogo extends StatelessWidget {
  const StoreLogo({this.onTap, this.storeLogo, this.storeName, key})
      : super(key: key);
  final String storeName;
  final String storeLogo;
  final Function onTap;
  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Column(
        children: [
          SizedBox(
            height: 20,
          ),
          Container(
            width: 200,
            child: Image.asset(
              storeLogo,
              height: 100,
            ),
            decoration: BoxDecoration(
                border: Border.all(width: 1, color: Colors.blue),
                borderRadius: BorderRadius.circular(10)),
          ),
          SizedBox(
            height: 8,
          ),
          Text(
            storeName ?? "",
            style: TextStyle(color: Colors.grey),
          )
        ],
      ),
    );
  }
}
