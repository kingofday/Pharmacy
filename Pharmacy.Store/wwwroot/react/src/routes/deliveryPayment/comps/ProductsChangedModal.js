import React from 'react';
import { Modal, Button } from 'react-bootstrap';
import strings from './../../../shared/constant';

export default class ProductsChangedModal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            show: false
        };
    }

    _toggle() {
        this.setState(p => ({ ...p, show: !p.show }));
    }

    _continue() {
       if(this.props.continue)this.props.continue();
    }

    _basket(){
        if(this.props.goToBasket)this.props.goToBasket();
    }

    render() {
        return (
            <Modal
                show={this.state.show}
                onHide={() => this._toggle()}
                centered
                size='lg'
                dialogClassName="modal-90w"
                aria-labelledby="modal-title"
                className='products-changed-modal'>
                <Modal.Header closeButton>
                    <Modal.Title>
                        {strings.attention}
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {strings.basketProductsChanged}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" className='btn-continue' onClick={this._continue.bind(this)}>
                        {strings.continue}
                    </Button>
                    <Button variant="warning" className='btn-basket' onClick={this._basket.bind(this)}>
                        {strings.review}
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    }
}