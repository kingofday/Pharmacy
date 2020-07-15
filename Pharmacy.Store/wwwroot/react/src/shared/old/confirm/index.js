import React from 'react';
import { Modal, Button } from 'react-bootstrap';
import strings from './../../shared/constant';

export default class ConfirmModal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show: false
        };
    }

    _toggle(id, body) {
        this.setState(p => ({ ...p, show: !p.show, id: id, body: body ? body : '' }))
    }
    _delete() {
        this.props.onDelete(this.state.id)
        this.setState(p => ({ ...p, show: false }));
    }
    render() {
        return (
            <Modal
                show={this.state.show}
                onHide={() => this._toggle()}
                centered
                size='sm'
                dialogClassName="modal-90w"
                aria-labelledby="modal-title"
                className='confirm-modal'
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        {this.props.title}
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {this.state.body}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="danger" onClick={this._delete.bind(this)}>
                        {strings.delete}
                    </Button>
                    <Button variant="secondary" onClick={this._toggle.bind(this)}>
                        {strings.cancel}
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    }
}
