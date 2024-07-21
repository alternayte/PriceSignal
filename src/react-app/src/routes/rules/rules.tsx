import {ContentLayout} from "@/components/layouts/content"
import { CreateRule } from "@/features/rules/components/create-rule"
import { RulesList } from "@/features/rules/components/rules-list"

export const RulesRoute = () => {
    return (
        <ContentLayout title='Rules'>
            <div className='flex justify-end'>
                <CreateRule/>
            </div>
            <div className='mt-4'>
                <RulesList/>
            </div>
        </ContentLayout>
    )
}