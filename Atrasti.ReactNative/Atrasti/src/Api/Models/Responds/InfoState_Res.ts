
import { InfoCity_Res } from './InfoCity_Res';

export interface InfoState_Res {
    state: string;
    cities: { [key: string]: InfoCity_Res };
}