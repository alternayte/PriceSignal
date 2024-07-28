import { Switch } from "@/components/ui/switch";
import { graphql } from "@/gql";
import { PriceRule } from "@/gql/graphql";
import { useMutation } from "@apollo/client";

const toggleRuleStatus = graphql(`
    mutation ToggleRuleStatus($id: UUID!) {
        togglePriceRule(id:$id) {
            id
            isEnabled
        }
    }
`)


type RuleToggleProps = {
    rule: PriceRule;
}
export const RuleToggle = ({rule}:RuleToggleProps) => {
    const [toggleRule] = useMutation(toggleRuleStatus);
    const handleToggle = () => {
        toggleRule({variables:{id:rule.id}})
    }
    return (
        <div>
            <Switch id={`rule-${rule.id}-status`} onCheckedChange={handleToggle} defaultChecked={rule.isEnabled} />
        </div>
    );
};

