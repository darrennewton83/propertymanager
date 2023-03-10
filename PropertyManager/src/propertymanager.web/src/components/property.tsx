import { useCallback, useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';
import { useForm } from "react-hook-form";
import { literal, number, object, string, TypeOf, z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { PropertyInput, propertySchema } from '../schemas/propertySchema';
import { PropertyType } from '../schemas/propertyTypeSchema';
import {
    Box,
    Button,
    Card,
    CardActions,
    CardContent,
    CardHeader,
    Checkbox,
    FormControlLabel,
    Divider,
    Select,
    Stack,
    Switch,
    TextField,
    Unstable_Grid2 as Grid,
    MenuItem
} from '@mui/material';
import * as React from 'react';

export const PropertyDetails = () => {
    const queryString = useParams();
    const navigate = useNavigate();
    const [values, setValues] = useState<PropertyInput>({
        id: null,
        propertyTypeId: null,
        propertyTypeName: '',
        addressLine1: '',
        addressLine2: '',
        city: '',
        region: '',
        postalCode: '',
        purchasePrice: null,
        purchaseDate: null,
        garage: false,
        numberOfParkingSpaces: null,
        notes: ''
    });
    const [propertyTypes, setPropertyTypes] = useState([
    ])

    const {
        register,
        formState: { errors, isSubmitSuccessful },
        reset,
        handleSubmit,
    } = useForm<PropertyInput>({
        resolver: zodResolver(propertySchema),
        values
    });

    const handleChange = useCallback(
        (event) => {
            setValues((prevState) => ({
                ...prevState,
                [event.target.name]: event.target.value
            }));
        },
        []
    );

    const handleToggleChange = useCallback(
        (event) => {
            setValues((prevState) => ({
                ...prevState,
                [event.target.name]: event.target.checked
            }));
        },
        []
    );
    const onSubmit = (data) => {
        

        axios.post('https://localhost:7103/Property', values)
            .then(() => { navigate('../properties'); })
            .catch(error => { alert(error); })

        

    }

    
    
    
    useEffect(() => {
        (async () => {
            let id: number = 0;

            if (Number.isInteger(Number(queryString.id))) {
                id = Number(queryString.id);
            }

            await axios.get('https://localhost:7103/PropertyType').then(function (response) {
                setPropertyTypes(response.data);
                if (response.data.length > 0 && values.propertyTypeId == null) {
                    values.propertyTypeId = response.data[0].id;
                }
            }).catch(function (error) {
                alert(error);
            });           

            if (id > 0) //retrieve record and populate data
            {
                await axios.get('https://localhost:7103/Property/' + id).then(function (response) {
                    setValues(response.data);
                }).catch(function (error) {
                        alert(error);
                        
                });
            }
        }
            
        )();
    }, []);

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={3}>
            <Card>
                <CardHeader
                    subheader="The address of the property"
                    title="Address"
                />
                <CardContent sx={{ pt: 0 }}>
                    <Box sx={{ m: -1.5 }}>
                        <Grid
                            container
                            spacing={3}
                        >
                            <Grid
                                xs={12}
                                md={6}
                            >
                                    <TextField
                                        {...register("addressLine1")}
                                    fullWidth
                                    label="Line 1"
                                    name="addressLine1"
                                    onChange={handleChange}
                                    value={values.addressLine1}
                                    />
                                    {errors.addressLine1 && (
                                        <p className="text-xs italic text-red-500 mt-2">
                                            {errors.addressLine1?.message}
                                        </p>
                                    )}
                            </Grid>
                            <Grid
                                xs={12}
                                md={6}
                            >
                                <TextField
                                    fullWidth
                                    label="Line 2"
                                    name="addressLine2"
                                    onChange={handleChange}
                                    value={values.addressLine2}
                                />
                            </Grid>
                            <Grid
                                xs={12}
                                md={6}
                            >
                                    <TextField
                                        {...register("city", {required: true})}
                                    fullWidth
                                    label="City"
                                    name="city"
                                    onChange={handleChange}
                                    value={values.city}
                                    />
                                    {errors.city && (
                                        <p className="text-xs italic text-red-500 mt-2">
                                            {errors.city?.message}
                                        </p>
                                    )}
                            </Grid>
                            <Grid
                                xs={12}
                                md={6}
                            >
                                <TextField
                                    fullWidth
                                    label="County"
                                    name="region"
                                    onChange={handleChange}
                                    value={values.region}
                                />
                            </Grid>
                            <Grid
                                xs={12}
                                md={6}
                            >
                                <TextField
                                    fullWidth
                                    label="Postcode"
                                    name="postalCode"
                                    onChange={handleChange}
                                    value={values.postalCode}
                                    />
                                    {errors.postalCode && (
                                        <p className="text-xs italic text-red-500 mt-2">
                                            {errors.postalCode?.message}
                                        </p>
                                    )}
                            </Grid>
                        </Grid>
                    </Box>
                </CardContent>
            </Card>
            <Card>
                <CardHeader
                    title="General Information"
                />
                <CardContent sx={{ pt: 0 }}>
                    <Box sx={{ m: -1.5 }}>
                        <Grid
                            container
                            spacing={3}
                        >
                            <Grid
                                xs={12}
                                md={6}
                            >
                                    <TextField
                                        {...register("propertyTypeId", { valueAsNumber: true })}
                                    fullWidth
                                    label="Property Type"
                                    name="propertyTypeId"
                                    onChange={handleChange}
                                    select
                                    SelectProps={{ native: true }}
                                    value={values.propertyTypeId}
                                >
                                    {propertyTypes.map((option: PropertyType) => (
                                        <option
                                            key={option.id}
                                            value={option.id}
                                        >
                                            {option.name}
                                        </option>
                                    ))}
                                </TextField>
                                    {errors.propertyTypeId && (
                                        <p className="text-xs italic text-red-500 mt-2">
                                            {errors.propertyTypeId?.message}
                                        </p>
                                    )}
                            </Grid>
                            <Grid
                                xs={12}
                                md={6}
                            >
                                    <TextField
                                        {...register("purchaseDate", { valueAsDate: true })}
                                    fullWidth
                                    label="Purchase Date"
                                    name="purchaseDate"
                                    onChange={handleChange}
                                    value={values.purchaseDate}
                                    type="date"
                                />
                                </Grid>
                                
                            <Grid
                                xs={12}
                                md={6}
                            >
                                    <TextField
                                        {...register("purchasePrice", {valueAsNumber: true})}
                                    fullWidth
                                    label="Purchase Price"
                                    name="purchasePrice"
                                    onChange={handleChange}
                                    value={values.purchasePrice}
                                        type="number"
   
                                    />
                                    {errors.purchasePrice && (
                                        <p className="text-xs italic text-red-500 mt-2">
                                            {errors.purchasePrice?.message}
                                        </p>
                                    )}
                                </Grid>
                                <Grid
                                    xs={12}
                                    md={6}
                                >
                                    <FormControlLabel labelPlacement="start" control={<Switch checked={values.garage} name="garage" onChange={handleToggleChange} />} label="Garage Included" />
                                </Grid>
                                <Grid
                                    xs={12}
                                    md={6}
                                >
                                    <TextField
                                        {...register("numberOfParkingSpaces", { valueAsNumber: true })}
                                        fullWidth
                                        label="Number of parking spaces"
                                        name="numberOfParkingSpaces"
                                        onChange={handleChange}
                                        value={values.numberOfParkingSpaces}
                                        type="number"

                                    />
                                    {errors.numberOfParkingSpaces && (
                                        <p className="text-xs italic text-red-500 mt-2">
                                            {errors.numberOfParkingSpaces?.message}
                                        </p>
                                    )}
                                </Grid>
                        </Grid>
                    </Box>
                </CardContent>
            </Card>
            <Card>
                <CardHeader
                    title="Notes"
                    subHeader="Any notes you would like to record about the property"
                />
                <CardContent sx={{ pt: 0 }}>
                    <Box sx={{ m: -1.5 }}>
                        <Grid
                            container
                            spacing={3}
                        >
                            <Grid
                                xs={24}
                                md={12}
                            >
                            <TextField
                                fullWidth
                                name="notes"
                                onChange={handleChange}
                                value={values.notes}
                                multiline
                                rows={4}
                                />
                            </Grid>
                        </Grid>
                    </Box>
                </CardContent>
                </Card>
                <Stack spacing={1} direction="row">
                    <Button type="submit" variant="contained" color="primary">
                
                        Save
                    </Button><Button color="secondary" onClick={() => { navigate('../properties ') } }>

                        Cancel
                    </Button></Stack>
            </Stack>
        </form>
    )
}