import { ComponentFactoryResolver, ComponentRef, Directive, Input, OnInit, ViewContainerRef } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { ClientSettingCustomAttributeContract } from '../../../models/custom-forms/custom-attribute';
import { InputComponent } from '../../custom-component/input-text/input.component';
import { InputNumericComponent } from '../../custom-component/input-numeric/input.numeric.component';
import { BooleanComponent } from '../../custom-component/boolean/booleans.component';
import { DateComponent } from '../../custom-component/date/date.component';
const componentMapper = {
    CADTDAT: DateComponent,
    CADTTEX: InputComponent,
    CADTNUM: InputNumericComponent,
    CADTBLN: BooleanComponent
};


@Directive({
    selector: '[dynamicField]'
})

export class DynamicFieldDirective {
    @Input() field: ClientSettingCustomAttributeContract;
    @Input() group: FormGroup;
    componentRef: any;
    constructor(private resolver: ComponentFactoryResolver,
        private container: ViewContainerRef
    ) { }
    ngOnInit() {
        const factory = this.resolver.resolveComponentFactory(
            componentMapper[this.field.CustomAttributeDatatypeCode]
        );
        this.componentRef = this.container.createComponent(factory);
        this.componentRef.instance.field = this.field;
        this.componentRef.instance.group = this.group;
    }
}