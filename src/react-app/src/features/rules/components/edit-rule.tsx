import {Button} from "@/components/ui/button"
import {
    Drawer,
    DrawerClose,
    DrawerContent,
    DrawerDescription,
    DrawerFooter,
    DrawerHeader,
    DrawerTitle,
    DrawerTrigger
} from "@/components/ui/drawer"
import { Textarea } from "@/components/ui/textarea"
import {Form, FormLabel, FormItem, FormControl, FormMessage, FormField} from "@/components/ui/form";
import {cn} from "@/lib/utils";
import * as React from "react";
import {forwardRef, useEffect, useRef, useState} from "react";
import {zodResolver} from "@hookform/resolvers/zod"
import {useFieldArray, useForm} from "react-hook-form";
import {z} from "zod"
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select";
import { Input } from "@/components/ui/input";
import { graphql } from "@/gql";
import {useMutation, useQuery} from "@apollo/client";
import {ConditionType, InstrumentsEdge } from "@/gql/graphql";

let indicators: [string,...string[]] = ['RSI'];
const directions = ['Above', 'Below'] as [string, ...string[]]

const createRuleMutation = graphql(`
    mutation CreatePriceRule($newRule: PriceRuleInput!) {
        createPriceRule(input:$newRule) {
            id
            name
            description
        }
    }
`)

const getInstrumentsQuery = graphql(`
    query GetInstruments($first: Int!) {
        instruments(first:$first) {
            totalCount
            edges {
                node {
                symbol
                id
                }
            }
        }
    }
`)
export const EditRule = () => {
    const [open, setOpen] = useState(false);
    const [hasUnsavedChanges, setHasUnsavedChanges] = useState(false);
    const [createRule] = useMutation(createRuleMutation, {onCompleted: ()=> {
        setHasUnsavedChanges(false)
        setOpen(false)}})
    const formRef = useRef<HTMLFormElement>(null);
    const {loading, error, data:instrumentsData} = useQuery(getInstrumentsQuery,{
        variables: {
            first: 5
        }
    })
    
    indicators = ['RSI', 'MACD', 'SMA', 'EMA']
    
    const handleOpenChange = (open: boolean) => {
        if (!open && hasUnsavedChanges) {
            if (!confirm('Are you sure you want to discard your changes?')) {
                return;
            } else {
                setHasUnsavedChanges(false);
            }
        }
        setOpen(open);
    };
    
    const handleSubmit = () => {
        if (formRef.current) {
            formRef.current.dispatchEvent(new Event('submit', { cancelable:true, bubbles:true}))
        }
    }
    
    const onSubmit = (data: z.infer<typeof CreateRuleFormSchema>) => {
        console.log('submitting', data)
        createRule({
            variables: {
                newRule: {
                    name:data.name,
                    description:data.description!,
                    instrumentId:data.symbol,
                    conditions:data.conditions.map(condition => {
                        return {
                            conditionType: condition.conditionType,
                            value: condition.threshold, 
                            additionalValues: JSON.stringify({name: condition.indicator, period: condition.period, direction: condition.direction}),
                        }}) || [],
                    }}})
              
    }
    
    if (loading) return <div>Loading...</div>
    if (error) return <div>Error</div>
    
    return (
        <Drawer direction='left' open={open} onOpenChange={handleOpenChange} dismissible={false}>
            <DrawerTrigger asChild>
                <Button>Create Rule</Button>
            </DrawerTrigger>
            <DrawerContent className='h-full w-full lg:w-1/2'>
                <DrawerHeader className='text-left'>
                    <DrawerTitle>Create Rule</DrawerTitle>
                    <DrawerDescription>
                        Create a new rule for your application
                    </DrawerDescription>
                </DrawerHeader>
                <RuleForm ref={formRef} className='px-4' changeTracker={setHasUnsavedChanges} indicators={indicators} symbols={instrumentsData?.instruments?.edges || []} submitData={onSubmit}/>
                <DrawerFooter className='pt-2'>
                    <div className="flex items-center space-x-2">
                        <DrawerClose asChild>
                            <Button className="w-fit">
                                Cancel
                            </Button>
                        </DrawerClose>
                        <Button className="w-fit" onClick={() => handleSubmit()}>
                            Submit
                        </Button>
                    </div>
                </DrawerFooter>
            </DrawerContent>
        </Drawer>
    )
}

interface RuleFormProps extends React.ButtonHTMLAttributes<HTMLElement> {
    changeTracker: (value: boolean) => void;
    symbols: InstrumentsEdge[];
    indicators: string[];
    submitData: (data: z.infer<typeof CreateRuleFormSchema>) => void;
}

const RuleForm = forwardRef<HTMLFormElement,RuleFormProps>(({className, changeTracker, indicators, symbols, submitData},ref) => {
    const createRuleForm = useForm<z.infer<typeof CreateRuleFormSchema>>({
        resolver: zodResolver(CreateRuleFormSchema),
        defaultValues: {},
    })
    
    const {fields, append, remove } = useFieldArray({
        control: createRuleForm.control,
        name: 'conditions'
    })

    useEffect(() => {
        const subscription = createRuleForm.watch((_values) => {
                changeTracker(true)
            }
        );
        return () => subscription.unsubscribe();
    }, [createRuleForm.watch]);


    const onSubmit = (data: z.infer<typeof CreateRuleFormSchema>) => {
        submitData(data)
    }

    const { errors } = createRuleForm.formState;

    return (
        <div className={cn(className)}>
            <Form {...createRuleForm}>
                <form ref={ref} onSubmit={createRuleForm.handleSubmit(onSubmit)} className='space-y-8'>
                    <FormField control={createRuleForm.control} name='name'
                               render={({field}) => (
                                   <FormItem>
                                       <FormLabel className=''>Name</FormLabel>
                                       <Input {...field} type='text' placeholder='[rule name]'
                                              className='input'/>
                                       <FormMessage/>
                                   </FormItem>
                               )}
                    />
                    <FormField control={createRuleForm.control} name='description'
                               render={({field}) => (
                                   <FormItem>
                                       <FormLabel className=''>Description</FormLabel>
                                       <Textarea {...field} placeholder='[rule descriprion]'
                                              className='input'/>
                                       <FormMessage/>
                                   </FormItem>
                               )}
                    />
                    <FormField control={createRuleForm.control} name='symbol'
                               render={({field}) => (
                                   <FormItem>
                                       <FormLabel className='sr-only'>Symbol</FormLabel>
                                       {/*<Input {...field} type='text' placeholder='Symbol' className='input'/>*/}
                                       <Select onValueChange={field.onChange} defaultValue={field.value}>
                                           <FormControl>
                                               <SelectTrigger>
                                                   <SelectValue placeholder="[Trade pair]"/>
                                               </SelectTrigger>
                                           </FormControl>
                                           <SelectContent>
                                               {symbols.map((item) => (
                                                   <SelectItem key={item.node.id} value={item.node.id}>
                                                       {item.node.symbol}
                                                   </SelectItem>
                                               ))}
                                           </SelectContent>
                                       </Select>
                                       <FormMessage/>
                                   </FormItem>
                               )}
                    />
                    {fields.map((_field, index)=> (
                        <div className='space-y-4 flex flex-row flex-wrap gap-3 items-baseline'>
                            <span className='flex-0'>When</span>
                            
                            <FormField control={createRuleForm.control} name={`conditions.${index}.conditionType`}
                                       render={({field}) => (
                                           <FormItem>
                                               <FormLabel className='sr-only'>Type</FormLabel>
                                               <Select onValueChange={field.onChange} defaultValue={field.value}>
                                                   <FormControl>
                                                       <SelectTrigger>
                                                           <SelectValue placeholder="[condition]"/>
                                                       </SelectTrigger>
                                                   </FormControl>
                                                   <SelectContent>
                                                       {Object.entries(ConditionType).map(([key,value]) => (
                                                           <SelectItem key={key} value={value}>
                                                               {key.replace(/_/g,' ')}
                                                           </SelectItem>
                                                       ))}
                                                   </SelectContent>
                                               </Select>
                                               <FormMessage/>
                                           </FormItem>
                                       )}
                            />
                            <span className='flex-0'>has</span>
                            <FormField control={createRuleForm.control} name={`conditions.${index}.indicator`}
                                       render={({field}) => (
                                           <FormItem>
                                               <FormLabel className='sr-only'>Indicator</FormLabel>
                                               <Select onValueChange={field.onChange} defaultValue={field.value}>
                                                   <FormControl>
                                                       <SelectTrigger>
                                                           <SelectValue placeholder="[indicator]"/>
                                                       </SelectTrigger>
                                                   </FormControl>
                                                   <SelectContent>
                                                       {indicators.map((item) => (
                                                           <SelectItem key={item} value={item}>
                                                               {item}
                                                           </SelectItem>
                                                       ))}
                                                   </SelectContent>
                                               </Select>
                                               <FormMessage/>
                                           </FormItem>
                                       )}
                            />
                            <FormField control={createRuleForm.control} name={`conditions.${index}.period`}
                                       render={({ field }) => (
                                           <FormItem>
                                               <FormLabel className='sr-only'>Period</FormLabel>
                                               <Input {...field} type='number' placeholder='12' className='input w-20' />
                                               <FormMessage />
                                           </FormItem>
                                       )}
                            />
                            <span className='flex-0'>going</span>
                            <FormField control={createRuleForm.control} name={`conditions.${index}.direction`}
                                       render={({field}) => (
                                           <FormItem>
                                               <FormLabel className='sr-only'>Direction</FormLabel>
                                               <Select onValueChange={field.onChange} defaultValue={field.value}>
                                                   <FormControl>
                                                       <SelectTrigger>
                                                           <SelectValue placeholder="[direction]"/>
                                                       </SelectTrigger>
                                                   </FormControl>
                                                   <SelectContent>
                                                       {directions.map((item) => (
                                                           <SelectItem key={item} value={item}>
                                                               {item}
                                                           </SelectItem>
                                                       ))}
                                                   </SelectContent>
                                               </Select>
                                               <FormMessage/>
                                           </FormItem>
                                       )}
                            />
                            <FormField control={createRuleForm.control} name={`conditions.${index}.threshold`}
                                       render={({field}) => (
                                           <FormItem>
                                               <FormLabel className='sr-only'>Threshold</FormLabel>
                                               <Input {...field} type='number' placeholder='30'
                                                      className='input w-20'/>
                                               <FormMessage/>
                                           </FormItem>
                                       )}
                            />
                            <Button type="button" onClick={() => remove(index)}>-</Button>

                        </div>
                    ))}
                    {errors.conditions && (
                        <div className="text-red-500">
                            {errors.conditions.message}
                        </div>
                    )}
                    <Button type='button' onClick={() => append({})}>Add condition</Button>
                </form>
            </Form>
        </div>
    )
});

const CreateRuleFormSchema = z.object({
    name: z.string(),
    description: z.string().optional(),
    symbol: z.string().uuid("Must be a valid identifier"),
    conditions: z.array(z.object({
        
        conditionType: z.nativeEnum(ConditionType),
        indicator: z.enum(indicators).optional(),
        period: z.coerce.number().int().positive(),
        direction: z.enum(directions),
        threshold: z.coerce.number().int().positive(),
    })).min(1,{message: "At least one condition is required"}).refine(conditions => conditions.every(condition => {
        if (condition.conditionType === "TECHNICAL_INDICATOR") {
            return condition.indicator !== undefined
        }
        return true;
    }), {
        message: 'Indicator is required when condition type is "Technical Indicator"',
        path: ['conditions']
    }),
});
