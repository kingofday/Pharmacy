import actionTypes from './actionTypes';
export function SetTempBasketIdAction(id) {
    return {
        type: actionTypes.SET_BASKET_ID,
        payload: {
            basketId: id
        }
    };
};
export function ClearTempBasketAction() {
    return {
        type: actionTypes.CLEAR_TEMP_BASKET
    };
};