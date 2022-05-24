import React from "react";
import { StyleSheet, View } from "react-native";
import { Picker } from "@react-native-picker/picker";
import { BaseCategory_Res } from "../Api/Models/Responds/BaseCategory_Res";

export interface AtrastiPickerProps {
    data: BaseCategory_Res[];
    onValueChange: (value: number, index: number) => void,
    selectedValue: number
}

export default class CategoryPicker extends React.Component<AtrastiPickerProps, any> {

    constructor(props: AtrastiPickerProps) {
        super(props);
    }

    renderProductList() {
        return this.props.data.map((label, index) => {
            return <Picker.Item key={index} label={label.title} value={label.id}/>
        })
    }

    render() {
        return (
            <View style={Styles.parent}>
                <Picker style={Styles.selectPicker}
                        selectedValue={this.props.selectedValue}
                        onValueChange={this.props.onValueChange}>
                    {this.renderProductList()}
                </Picker>
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    parent: {
        marginTop: 10,
        overflow: "hidden",
        borderRadius: 50,
        flex: 1
    },
    selectPicker: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        color: '#FFFFFF',
        padding: 10
    },
});
