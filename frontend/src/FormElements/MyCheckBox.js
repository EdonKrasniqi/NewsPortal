import { useField } from 'formik';
import React from 'react';
import { Form, Label } from 'semantic-ui-react';



export default function MyCheckBox(props){
    const [field, meta] = useField(props.name);
    return (
        <Form.Field error={meta.touched && !!meta.error}>
           
            <div class="inline field">
    <div class="ui toggle checkbox">
            <input type="checkbox"  {...field}  {...props} />
            <label>{props.label}</label>
            {meta.touched && meta.error ? (
                <Label basic color='red'> {meta.error} </Label>
            ) : null}
            </div>
  </div>
        </Form.Field>
    )
}