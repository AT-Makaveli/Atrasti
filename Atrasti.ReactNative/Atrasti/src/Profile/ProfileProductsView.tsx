import React from "react";
import { Image, StyleSheet, Text, TouchableOpacity, View } from "react-native";
import { getWindowDimensions } from "../utils/WindowUtils";
import { getProductImage } from "../Api/APIData";
import { Product_Res } from "../Api/Models/Responds/Product_Res";

interface ProfileProductsProps {
    products: Product_Res[],
    modalChange: (display: boolean, index: number) => void
}

const windowDimensions = getWindowDimensions();
const windowWidth = windowDimensions.width;

const productImageWidth = windowWidth / 2;

export default class ProfileProductsView extends React.Component<ProfileProductsProps> {

    constructor(props: ProfileProductsProps) {
        super(props);
    }

    render() {
        const renderProduct = (data: Product_Res, index: number) => {
            return (
                <TouchableOpacity activeOpacity={0.6} style={Styles.item} key={index} onPress={() => this.props.modalChange(true, index)}>
                    <View style={{
                        flex: 1, flexDirection: 'column'
                    }}>
                        <Image source={{uri: getProductImage(data.id)}} style={{
                            width: productImageWidth - 12,
                            height: productImageWidth - 12,
                            borderRadius: 20,
                            marginLeft: 6,
                        }}/>
                        <Text style={{
                            textAlign: "center",
                            color: "white"
                        }}>
                            {data.title}
                        </Text>
                    </View>
                </TouchableOpacity>
            );
        };

        return (
            <View style={{flexDirection: 'row', flexWrap: 'wrap'}}>
                {this.props.products.map((value, index) => {
                    return renderProduct(value, index);
                })}
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    item: {
        width: '50%',
        flexDirection: 'row'
    },
});
