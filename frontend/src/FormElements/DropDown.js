import { useField } from 'formik';
import React from 'react';
import { Form, Label, Select } from 'semantic-ui-react';



export default function MySelectInput(props){
    const [field, meta,helpers] = useField(props.name);
    return (
        <Form.Field error={meta.touched && !!meta.error}>
            <label>{props.label}</label>
            <Select 
            clerable="true"
            options={props.options}
            value={field.value || null}
            onChange={(o,e)=> helpers.setValue(e.value)}
            onBlur={()=>helpers.setTouched(true)}
            placeholder={props.placeholder}/>
            {meta.touched && meta.error ? (
                <Label basic color='red'> {meta.error} </Label>
            ) : null}
        </Form.Field>
    )
}