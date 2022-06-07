import React from "react";
import { Picker } from "@react-native-picker/picker";
import { StyleSheet, View } from "react-native";
import { InfoCity_Res } from "../../Api/Models/Responds/InfoCity_Res";
import { findCity, findCountry, findState } from "../../Api/CountryAPI";

export interface CityPickerProps {
    country: string,
    state: string,
    city: string,
    onValueChange: (country: string) => void
}

export interface CityPickerState {
    cities: InfoCity_Res[]
}

export default class CityPicker extends React.Component<CityPickerProps, CityPickerState> {

    constructor(props: CityPickerProps) {
        super(props);

        this.state = {
            cities: [],
        }
    }

    componentDidUpdate(prevProps: Readonly<CityPickerProps>, prevState: Readonly<CityPickerState>, snapshot?: any) {
        if (this.props.state !== prevProps.state) {
            this.updateCities();
            return;
        }
    }

    componentDidMount() {
        this.updateCities();
    }

    updateCities() {
        const state = findState(this.props.country, this.props.state);
        if(state === null) {
            this.setState({
                cities: []
            });
            this.updateCity('');
            return;
        }

        this.setState({
            cities: Object.values(state.cities)
        });

        const oldCity = findCity(this.props.country, this.props.state, this.props.city);
        if (oldCity !== null) {
            this.updateCity(oldCity.cityName);
            return;
        }

        const firstCity = Object.values(state.cities)[0];
        if(firstCity !== undefined) {
            this.updateCity(firstCity.cityName);
        }
    }

    updateCity(city: string) {
        this.props.onValueChange(city);
    }

    renderCities() {
        return this.state.cities.map((city, index) => {
            return <Picker.Item key={index} label={city.cityName} value={city.cityName}/>
        })
    }

    render() {
        return (
            <View style={Styles.parent}>
                <Picker style={Styles.selectPicker} selectedValue={this.props.city}
                        onValueChange={this.updateCity.bind(this)}>
                    {this.renderCities()}
                </Picker>
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    parent: {
        marginTop: 10,
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
