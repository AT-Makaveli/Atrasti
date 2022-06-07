export default class CountryPickerEvent {
    public static EVENT_TYPE: "COUNTRY_PICKER_SELECTED";

    public readonly country: string;

    constructor(country: string) {
        this.country = country;
    }
}
