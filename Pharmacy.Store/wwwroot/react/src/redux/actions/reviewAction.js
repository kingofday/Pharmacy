import actionTypes from './actionTypes';

export function SetAddrssAction(addr) {
    return {
        type: actionTypes.SET_ADDRESS,
        payload: { ...addr }
    };
};

export function SetDeliveryType(delivery, comment) {
    return {
        type: actionTypes.SET_DELIVERY_TYPE,
        payload: { delivery: { ...delivery }, comment }
    };
};