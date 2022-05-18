import React from "react";
import { Picker } from "@react-native-picker/picker";
import { DeviceEventEmitter, EmitterSubscription, StyleSheet, View } from "react-native";
import { findCountry} from "../Api/CountryAPI";
import { InfoCity_Res } from "../Api/Models/Responds/InfoCity_Res";
import CountryPickerEvent from "../utils/Events/Picker/CountryPickerEvent";

export interface CityPickerProps {
    country: string,
    state: string,
    city: string,
    onValueChange: (country: string) => void
}

export interface CityPickerState {
    cities: InfoCity_Res[],
}

export default class CityPicker extends React.Component<CityPickerProps, CityPickerState> {

    private _onCountryChange: EmitterSubscription | null = null;

    constructor(props: CityPickerProps) {
        super(props);

        this.state = {
            cities: []
        }
    }

    componentDidMount() {
        this._onCountryChange = DeviceEventEmitter.addListener(CountryPickerEvent.EVENT_TYPE, this.onCountryChange.bind(this));
        if(this.props.state === '') {
            this.setState({
                cities: [{
                    cityName: 'Select state first'
                }]
            })
        }

        const country = findCountry(this.props.country);
        if(country !== null && this.props.state !== undefined) {
            const state = country.states[this.props.state];
            if (state !== undefined && state.cities !== undefined) {
                this.setState({
                    cities: Object.values(state.cities)
                })
            }
        }
    }

    componentWillUnmount() {
        this._onCountryChange?.remove();
    }

    onCountryChange(event: CountryPickerEvent) {
        const country = event.country;
        const state = event.state;

        const countryModel = findCountry(country);
        if(countryModel !== null && state !== null) {
            const stateModel = countryModel.states[state];
            this.setState({
                cities: Object.values(stateModel.cities)
            });
        } else if(countryModel !== null && state === null) {

            const firstStateName = Object.keys(countryModel.states)[0];
            if(firstStateName === undefined) return;

            const firstStateInCountry = countryModel.states[firstStateName];
            if(firstStateInCountry === undefined) {
                this.setState({
                    cities: []
                });

                return;
            }

            this.setState({
                cities: Object.values(firstStateInCountry.cities)
            })
        }
    }

    renderProductList() {
        return this.state.cities.map((city, index) => {
            return <Picker.Item key={index} label={city.cityName} value={city.cityName}/>
        })
    }

    render() {
        return (
            <View style={Styles.parent}>
                <Picker style={Styles.selectPicker} selectedValue={this.props.city}
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
        borderRadius: 50
    },
    selectPicker: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        color: '#FFFFFF',
        padding: 10
    },
});
