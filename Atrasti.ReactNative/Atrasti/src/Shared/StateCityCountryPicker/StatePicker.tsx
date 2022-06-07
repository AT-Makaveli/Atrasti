import React from "react";
import { Picker } from "@react-native-picker/picker";
import { StyleSheet, View } from "react-native";
import { findCountry } from "../../Api/CountryAPI";

export interface StatePickerProps {
    country: string,
    state: string,
    onValueChange: (state: string) => void
}

export interface StatePickerState {
    states: string[],
    newCountry: string
}

export default class StatePicker extends React.Component<StatePickerProps, StatePickerState> {

    constructor(props: StatePickerProps) {
        super(props);

        this.state = {
            states: [],
            newCountry: ''
        }
    }

    componentDidUpdate(prevProps: Readonly<StatePickerProps>, prevState: Readonly<StatePickerState>, snapshot?: any) {
        if(prevProps.country !== this.props.country) {
            this.updateStates();
        }
    }

    componentDidMount() {
        this.updateStates();
    }

    updateStates() {
        const country = findCountry(this.props.country);
        if(country !== null) {
            const states = Object.keys(country.states);
            const firstState = states[0];
            if (firstState !== undefined && this.props.state === '') {
                this.props.onValueChange(firstState);
            }

            this.setState({
                states: states
            });
        } else {
            this.setState({
                states: []
            })
        }
    }

    renderProductList() {
        return this.state.states.map((state, index) => {
            return <Picker.Item key={index} label={state} value={state}/>
        })
    }

    render() {
        return (
            <View style={Styles.parent}>
                <Picker style={Styles.selectPicker} selectedValue={this.props.state ?? ''}
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
