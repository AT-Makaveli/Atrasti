import React from "react";
import { Picker } from "@react-native-picker/picker";
import { StyleSheet, View } from "react-native";
import { InfoCountry_Res } from "../Api/Models/Responds/InfoCountry_Res";
import { getCountries } from "../Api/CountryAPI";

export interface CountryPickerProps {
    country: string,
    onValueChange: (country: string) => void
}

export interface CountryPickerState {
    countries: InfoCountry_Res[]
}

export default class CountryPicker extends React.Component<CountryPickerProps, CountryPickerState> {
    constructor(props: CountryPickerProps) {
        super(props);

        this.state = {
            countries: []
        }
    }

    componentDidMount() {
        getCountries()
            .then(result => {
                this.setState({
                    countries: result
                })
            })
            .catch(error => {

            });
    }

    renderProductList() {
        return this.state.countries.map((country, index) => {
            return <Picker.Item key={index} label={country.country} value={country.country} />
        })
    }

    render() {
        return (
            <View style={Styles.parent}>
                <Picker style={Styles.selectPicker} selectedValue={this.props.country} onValueChange={this.props.onValueChange}>
                    {this.renderProductList()}
                </Picker>
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    parent: {
        overflow: "hidden",
        borderRadius: 50
    },
    selectPicker: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        color: '#FFFFFF',
        padding: 10
    },
});
