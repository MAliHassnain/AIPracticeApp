import { getStepDisplay } from './agent-step.helper';

describe('getStepDisplay', () => {
  it('should map known step kinds to a label and tone', () => {
    expect(getStepDisplay('web_search')).toEqual({ label: 'Search', tone: 'search' });
    expect(getStepDisplay('tool_result')).toEqual({ label: 'Tool result', tone: 'tool' });
  });

  it('should fall back to info for unknown kinds', () => {
    expect(getStepDisplay('something_new')).toEqual({ label: 'Info', tone: 'info' });
  });
});
